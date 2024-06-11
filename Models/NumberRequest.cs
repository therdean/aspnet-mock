namespace aspnet_mock.Models
{
    public class NumberRequest
    {
        public int Number { get; set; }
    }

    public class NumberResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public NumberResponse(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}