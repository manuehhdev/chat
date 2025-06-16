using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Chats
{
    public Guid Id { get; set; }

    public string Descripcion { get; set; } = null!;

    public bool EsGrupo { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual ICollection<ChatsUsuarios> ChatsUsuarios { get; set; } = new List<ChatsUsuarios>();
}
