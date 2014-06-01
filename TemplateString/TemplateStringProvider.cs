using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TemplateString
{
    public class TemplateStringProvider
    {
        public const char OpenBracket = '<';

        public const char ClosedBracket = '>';

        public Dictionary<string, string> Provide(string templateString, string valueString)
        {
            var templates = GetTemplates(templateString).ToList();

            var templateSeparators = GetTemplateSeparators(templateString, valueString, templates).ToList();

            var split = GetTemplatesValues(templateString, valueString, templateSeparators, templates.Count);

            var dictionary = new Dictionary<string, string>();

            for (int i = 0; i < split.Count(); i++)
            {
                dictionary.Add(templates[i].Trim(new[] { OpenBracket, ClosedBracket }), split[i]);
            }

            return dictionary;
        }

        private static List<string> GetTemplatesValues(
            string templateString,
            string valueString,
            IList<string> templateSeparators,
            int templatesCount)
        {
            if (templateSeparators.All(string.IsNullOrEmpty))
            {
                return new List<string>
                {
                    valueString
                };
            }

            var regex = new Regex(string.Concat(templateSeparators.Select(s => string.Format(@"{0}(.*?)", Regex.Escape(s)))));
            var templatesValues = regex.Split(valueString).Where(s => !string.IsNullOrEmpty(s)).ToList();

            if (templatesValues.Count != templatesCount)
            {
                throw new ArgumentException(
                    string.Format("Name {0} does not match the pattern {1}.", valueString, templateString));
            }

            return templatesValues;
        }

        private static IEnumerable<string> GetTemplateSeparators(string templateString, string name, IList<string> templates)
        {
            var strings =
                templateString.Split(templates.ToArray(), StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

            if (!strings.Any() && templates.Count() > 1)
            {
                throw new ArgumentException(
                    string.Format("Name {0} does not match the pattern {1}.", name, templateString));
            }

            return strings;
        }

        private static IEnumerable<string> GetTemplates(string templateString)
        {
            var regex = new Regex(
                string.Format(
                    @"{0}(.*?){1}",
                    Regex.Escape(
                        OpenBracket.ToString()),
                    Regex.Escape(
                        ClosedBracket.ToString())));

            return from Match m in regex.Matches(templateString) select m.Value;
        }
    }
}
