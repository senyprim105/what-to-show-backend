using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace server_api.Infrastructure
{
    public static class TempDataDictionaryExtensions
    {
        //Имя под которым сохраняются данные в TempData
        private const string _ModelStateErrorsKey = "ModelStateErrors";
        // Добавляем статические расширения для удобства
        public static IEnumerable<string> GetModelErrors(this ITempDataDictionary instance)
        {
            return TempDataDictionaryExtensions.GetErrorsFromTempData(instance);
        }

        public static void AddModelError(this ITempDataDictionary instance, string error)
        {
            TempDataDictionaryExtensions.AddModelErrors(instance, new List<string>() { error });
        }

        public static void AddModelErrors(this ITempDataDictionary instance, IEnumerable<string> errors)
        {
            TempDataDictionaryExtensions.AddErrorsToTempData(instance, errors);
        }
        // Возвращает из TempData определенный ключ в виде списка строк
        private static IList<string> GetErrorsFromTempData(ITempDataDictionary instance)
        {
            IList<string> tempErrors = new List<string>();
            if (instance.ContainsKey(TempDataDictionaryExtensions._ModelStateErrorsKey)){
                var obj = (instance[TempDataDictionaryExtensions._ModelStateErrorsKey]);
                var errors = obj as IList<string>;
                tempErrors = errors ?? tempErrors;
            } 
            return tempErrors;
        }

        // Добавляет в TempData список строк под ключом
        private static void AddErrorsToTempData(ITempDataDictionary instance, IEnumerable<string> errors)
        {
            var tempErrors = new List<string>();
            if (instance.ContainsKey(TempDataDictionaryExtensions._ModelStateErrorsKey)){
                tempErrors = (instance[TempDataDictionaryExtensions._ModelStateErrorsKey] as List<string>)??tempErrors;
            } 

            tempErrors.AddRange(errors);
            instance[TempDataDictionaryExtensions._ModelStateErrorsKey] = tempErrors;
        }
    }
}
