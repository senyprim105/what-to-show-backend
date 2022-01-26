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
   
    [HtmlTargetElement("checkbox-item")]
    public class CheckBoxItemHelper: TagHelper
    {
        private const string ChecboxSuffixAttr = "checkbox-item-";
        private const string CheckboxDataAttr = "checkbox-data";
        /*
         *checkbox-item-root-tag -tag корневого элемента
         *checkbox-item-root-class -class корневого элемента
         *checkbox-item-checkbox-class  -class корневого элемента
         *checkbox-item-label-class  -class корневого элемента
         */

        [HtmlAttributeName(DictionaryAttributePrefix = ChecboxSuffixAttr)]
        public Dictionary<string, string> CheckBoxItemAttribute { get; set; } = new Dictionary<string, string>();
        [HtmlAttributeName(CheckboxDataAttr)]
        public CheckBoxListViewModel CheckBoxItem{ get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            //Создает контейнер:
            //Если в атрибутах задан  root-tag то создается контаенер и и вешается на него класс и ид если есть
            //Иначе возвращается null
            TagBuilder CreateRoot()
            {
                var rootTag = CheckBoxItemAttribute.GetValue("root-tag");
                if (string.IsNullOrWhiteSpace(rootTag)) return null;
                var root = new TagBuilder(rootTag);
                if (CheckBoxItemAttribute.TryGetValue("root-class", out string rootClass)) root.AddCssClass(rootClass);
                return root;
            }

            TagBuilder CreateCheckBox(CheckBoxListViewModel item)
            {
                var checkbox = new TagBuilder("input");
                checkbox.MergeAttribute("type", "checkbox");
                checkbox.MergeAttribute("name", item.Name ?? "");
                checkbox.MergeAttribute("value", item.Value);
                if (item.Id != null) checkbox.MergeAttribute("id", item.Id);
                if (item.Checked??true) checkbox.MergeAttribute("checked", string.Empty);
                if (!item.Enabled ?? true) checkbox.MergeAttribute("disabled", string.Empty);
                if (!item.Visible ?? false) checkbox.MergeAttribute("hidden",string.Empty);
                if ((CheckBoxItemAttribute.GetValue("checkbox-class")) != null) checkbox.AddCssClass(CheckBoxItemAttribute.GetValue("checkbox-class"));
                return checkbox;
            }
            TagBuilder CreateLabel(CheckBoxListViewModel item)
            {
                var label = new TagBuilder("label");
                if (item.Id != null) label.MergeAttribute("for", item.Id);
                label.InnerHtml.Append(item.Caption??item.Value);
                if ((CheckBoxItemAttribute.GetValue("label-class")) != null) label.AddCssClass(CheckBoxItemAttribute.GetValue("label-class"));
                return label;
            }
            output.TagName = null;//Чтобы не выводить тег хелпера
            var item = CheckBoxItem?? context.Items["item"] as CheckBoxListViewModel;//Если данные заданны явно они в приоритете иначе берем данные из контекста

            var root = CreateRoot();
            var container = new TagBuilder("div");
            container.InnerHtml.AppendHtml(CreateCheckBox(item));
            container.InnerHtml.AppendHtml(CreateLabel(item));
            if (root == null)
            {
                output.Content.SetHtmlContent(container.InnerHtml);
            } else
            {
                root.InnerHtml.AppendHtml(container.InnerHtml);
                output.Content.SetHtmlContent(root);
            }
        }
    }
}
