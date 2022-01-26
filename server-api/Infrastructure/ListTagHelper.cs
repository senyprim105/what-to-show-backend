using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using server_api.Data.Models;
using server_api.Data.Models.Repositories;
using server_api.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace server_api.Infrastructure
{
   
    [HtmlTargetElement("iterator")]
    public class IteratorTagHelper: TagHelper
    {
        private const string iteratorSuffixAttr = "iterator-attr-";
        private const string iteratorDataAttr = "iterator-data";
        /*
         *iterator-attr-root-tag -тег корневого элемента
         *iterator-attr-root-class -class корневого элемента
         *iterator-attr-root-id -id корневого элемента
         *iterator-attr-item-tag -тег корневого элемента
         *iterator-attr-item-class -class корневого элемента
         */

        [HtmlAttributeName(DictionaryAttributePrefix = iteratorSuffixAttr)]
        public Dictionary<string, string> IteratorListAttribute { get; set; } = new Dictionary<string, string>();
        [HtmlAttributeName(iteratorDataAttr)]
        public IEnumerable<object> IteratorData { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            //Создает контейнер:
            //Если в атрибутах задан  root-tag то создается контаенер и и вешается на него класс и ид если есть
            //Иначе возвращается null
            TagBuilder createRoot()
            {
                var rootTag = IteratorListAttribute.GetValue("root-tag");
                if (string.IsNullOrWhiteSpace(rootTag)) return null;
                var root = new TagBuilder(rootTag);
                if (IteratorListAttribute.TryGetValue("root-class", out string rootClass)) root.AddCssClass(rootClass);
                if (IteratorListAttribute.TryGetValue("root-id", out string rootId)) root.MergeAttribute("id", rootId);
                return root;
            }
            //Создаем контейнер одного элемента (типа li)
            TagBuilder createItem(string tagName)
            {
                if (string.IsNullOrWhiteSpace(tagName)) return null;
                var item = new TagBuilder(tagName);
                if (IteratorListAttribute.TryGetValue("item-class", out string itemClass)) item.AddCssClass(itemClass);
                return item;
            }
         
            output.TagName = null;//Убираем тег хелпера
            var root = createRoot();//Создаем корневой элемент если задан
            var itemTag = IteratorListAttribute.GetValue("item-tag");//Получаем тег контейнера каждого элемента
            var isRootUlTag = root!=null && root.TagName == "ul";//Если корневой элемент список 
            itemTag = isRootUlTag ? "li" : itemTag;//то контейнер для элемента равен <li>
            
            var list = new TagBuilder("div");//Создаем контейнер для списка элементов
            foreach (var it in IteratorData)//Пробегаемся по списку
                context.Items["item"] = it;//Добавляем в контекст данные для детей
            {
                var item = createItem(itemTag??"div");//Создаем контейнер элемента
                var childContent = (await output.GetChildContentAsync(false)).GetContent();//Получаем содержимое детей
                item.InnerHtml.AppendHtml(childContent); //и добавляем детей в него

                list.InnerHtml.AppendHtml(
                    itemTag== null //Добавляем в контейнер списка 
                    ? item.InnerHtml //или содержимое элемента если элемент не предусмотрен
                    :item // или элемент если он предусмотрен атрибутами
                    );
            }

            if (root != null) //Если есть корневой элемент 
            {
                root.InnerHtml.AppendHtml(list.InnerHtml);//Добавляем в него содержимое контейнер для списка
                output.Content.SetHtmlContent(root);//и все это выводим 
            } else
            {
                output.Content.SetHtmlContent(list.InnerHtml);//Выводим содержимое контейнер для списка
            }
           
        }
    }
}
