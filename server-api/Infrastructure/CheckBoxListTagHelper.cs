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

    [HtmlTargetElement(Attributes = "checkbox-list")]
    public class CheckBoxListTagHelper : TagHelper
    {
        /*
         * checkbox-list-root-tag
         * checkbox-list-root-class
         * checkbox-list-root-id
         * checkbox-list-item-tag
         * checkbox-list-item-class
         * checkbox-list-checkbox-class
         * checkbox-list-label-class
        */
        [HtmlAttributeName(DictionaryAttributePrefix = "checkbox-list-")]
        public Dictionary<string, string> CheckBoxListAttribute { get; set; } = new Dictionary<string, string>();
        public IEnumerable<CheckBoxListViewModel> CheckboxList { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            //Создает контейнер:
            //Если в атрибутах задан  root-tag то создается контаенер и и вешается на него класс и ид если есть
            //Иначе возвращается null
            TagBuilder createRoot()
            {
                var rootTag = CheckBoxListAttribute.GetValue("root-tag");
                if (string.IsNullOrWhiteSpace(rootTag)) return null;
                var root = new TagBuilder(rootTag);
                if (CheckBoxListAttribute.TryGetValue("root-class", out string rootClass)) root.AddCssClass(rootClass);
                if (CheckBoxListAttribute.TryGetValue("root-id", out string rootId)) root.MergeAttribute("id", rootId);
                return root;
            }
            //Создаем кнтейнер если задан и ставим на него класс если задан
            TagBuilder createContainer(CheckBoxListViewModel checkBoxListData)
            {
                var containerTag = CheckBoxListAttribute.GetValue("item-tag");
                if (string.IsNullOrWhiteSpace(containerTag)) return null;
                var container = new TagBuilder(containerTag);
                if ((CheckBoxListAttribute.GetValue("item-class")) != null) container.AddCssClass(CheckBoxListAttribute.GetValue("item-class"));
                return container;
            }

            TagBuilder createCheckBox(CheckBoxListViewModel item)
            {
                var checkbox = new TagBuilder("input");
                checkbox.MergeAttribute("type", "checkbox");
                checkbox.MergeAttribute("name", item.Name ?? "");
                checkbox.MergeAttribute("value", item.Value);
                if (item.Id != null) checkbox.MergeAttribute("id", item.Id);
                if (item.Checked ?? true) checkbox.MergeAttribute("checked", string.Empty);
                if (!item.Enabled ?? true) checkbox.MergeAttribute("disabled", string.Empty);
                if (!item.Visible ?? false) checkbox.MergeAttribute("hidden", string.Empty);
                if ((CheckBoxListAttribute.GetValue("checkbox-class")) != null) checkbox.AddCssClass(CheckBoxListAttribute.GetValue("checkbox-class"));
                return checkbox;
            }
            TagBuilder createLabel(CheckBoxListViewModel item)
            {
                var label = new TagBuilder("label");
                if (item.Id != null) label.MergeAttribute("for", item.Id);
                label.InnerHtml.Append(item.Caption ?? item.Value);
                if ((CheckBoxListAttribute.GetValue("label-class")) != null) label.AddCssClass(CheckBoxListAttribute.GetValue("label-class"));
                return label;
            }
            void appendElement(TagHelperOutput output, params TagBuilder[] elements)
            {
                elements = elements.Where(element => element != null).ToArray();

                if (elements.Length > 1)
                {
                    elements[elements.Length - 2].InnerHtml.AppendHtml(elements[elements.Length - 1]);
                }
                else if (elements.Length == 1)
                {
                    output.Content.AppendHtml(elements[0]);
                }
                else
                {
                    return;
                }
            }

            var root = createRoot();

            foreach (var itemList in CheckboxList)
            {
                var item = createContainer(itemList);
                appendElement(output, root, item, createCheckBox(itemList));
                appendElement(output, root, item, createLabel(itemList));
                appendElement(output, root, item);
            }
            appendElement(output, root);
        }
    }
}
