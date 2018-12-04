
using System.Collections.Generic;
using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using markov_chain_generator_webapp.Models;
using markov_chain_generator_webapp.Extensions;

namespace markov_chain_generator_webapp.Controllers
{
    public class TextGeneratorController : Controller
    {

        Dictionary<string, string> sampleTextFiles = new Dictionary<string, string>
        {
            ["-----"] = "-----",
            ["Romeo and Juliet by William Shakespeare"] = "properties/romeo-and-juliet.txt",
            ["Horoscopes by Tarot.com"] = "properties/horoscopes.txt",
            ["Maroon 5 pop song lyrics"] = "properties/maroon-5.txt",
            ["Wikihow.com: How to Invite a Girl to Prom"] = "properties/wikihow.txt"
        };

        public IActionResult Index()
        {
            var model = GetActiveModelObject();
            ViewBag.samples = sampleTextFiles;
            return View(model);
        }

        private TextGeneratorData GetActiveModelObject()
        {
            TextGeneratorData model = null;
            if (HttpContext.Session.Get<TextGeneratorData>("UserInput") != null)
            {
                model = HttpContext.Session.Get<TextGeneratorData>("UserInput");
            }
            else{
                model = new TextGeneratorData();
                model.ModelOrder = 10;
                HttpContext.Session.Set("UserInput", model);
            }
            return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Generate(TextGeneratorData model)
        {
            MarkovGenerator<char> gen = new MarkovGenerator<char>(model.ModelOrder);
            bool doGenerate = true;

            string textFileInputString = sampleTextFiles[model.SampleInputChoice];
            if (model.SampleInputChoice != "-----")
            {
                try
                {
                    EnumerableTextFile file = new EnumerableTextFile(textFileInputString);
                    gen.AddEnumerator(file.GetEnumerator());
                }catch (Exception e)
                {
                    model.Output = "Error processing sample file. Please try again.";
                }
            }
            else
            {
                if (model.TextInput.Length < model.ModelOrder + 1)
                {
                    model.Output = "Not enough input provided (at least " + (model.ModelOrder + 1)+
                        " characters needed.)";
                    doGenerate = false;
                }
                else
                {
                    try
                    {
                        gen.AddEnumerator(model.TextInput.GetEnumerator());
                    }
                    catch (Exception e)
                    {
                        model.Output = "Error processing input. Please try again.";
                        doGenerate = false;
                    }
                }

            }
            if (doGenerate)
            {
                try
                {
                    gen.FinalizeGenerator();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < model.OutputLength; i++)
                    {
                        char next = gen.Generate();
                        sb.Append(next);
                    }
                    model.Output = sb.ToString();
                }
                catch (Exception e)
                {
                    model.Output = "Error generating output. Please try again.";
                }
            }
            HttpContext.Session.Set<TextGeneratorData>("UserInput", model);
            return RedirectToAction("Index");
        }
    }
}
