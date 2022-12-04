namespace DevStudyNotes.API.models
{
    public class AddStudyNoteInputModel
    {
        public AddStudyNoteInputModel(string title, string description, bool isPublic)
        {
            Title = title;
            Description = description;
            IsPublic = isPublic;
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsPublic { get; set; }



    }
}