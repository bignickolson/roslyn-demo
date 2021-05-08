using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using RoslynDemoAnalyzers.Generators.Models;
using Scriban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace RoslynDemoAnalyzers.Generators
{
    [Generator]
    public class LookupTableEnumGenerator : BaseGenerator
    {

        public override void Generate(GeneratorExecutionContext context)
        {

            var template = Template.Parse(EmbeddedResource.GetContent(@"Generators\Templates\LookupEnums.sbncs"), "LookupEnums.sbncs");
            var generatorData = new GeneratorData("TestNamespace");

            var lookupFile = context.AdditionalFiles.Where(i => i.Path.ToLower().Contains("lookups.xml")).SingleOrDefault();

            if (lookupFile != null)
            {
                var text = lookupFile.GetText(context.CancellationToken).ToString();
                var xDoc = XDocument.Parse(text);
                generatorData.LookupTables = xDoc.Root.Elements("LookupTable")
                    .Select(lt => new LookupTable(lt.Attribute("name").Value)
                    {
                        Values = lt.Elements("LookupValue").Select(lv => new LookupValue(lv.Attribute("name").Value, lv.Attribute("value").Value)).ToList()
                    }).ToList();

                var output = template.Render(generatorData, mr => mr.Name);
                context.AddSource("LookupEnums.cs", SourceText.From(output, Encoding.UTF8));
            }
        }

        public override void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}
