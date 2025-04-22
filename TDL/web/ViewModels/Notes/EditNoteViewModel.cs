using Domain.Entities;
    public class EditNoteViewModel
    {
            public int Id { get; set; }

            public string Title { get; set; }
            public string Content { get; set; }

            public List<int> SelectedTagIds { get; set; } = new();


    }
