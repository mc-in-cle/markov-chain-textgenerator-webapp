
namespace markov_chain_generator_webapp.Models
{
    public class TextGeneratorData
    {
        public string TextInput { get; set; }
        public string SampleInputChoice { get; set; }
        public int ModelOrder { get; set; }
        public int OutputLength { get; set; }
        public string Output { get; set; }

        public TextGeneratorData()
        {
            Output = "";
        }
    }
}
