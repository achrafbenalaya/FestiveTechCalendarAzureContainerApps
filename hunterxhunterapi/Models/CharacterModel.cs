namespace hunterxhunterapi.Models
{
    public class CharacterModel
    {
        public string Name { get; set; }
        public string Superpower { get; set; }
        public string ImageURL { get; set; }
    }

    public class CharacterList
    {
        public List<CharacterModel> Characters { get; set; }
    }
}
