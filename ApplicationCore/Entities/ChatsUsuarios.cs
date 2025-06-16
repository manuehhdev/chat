using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;
public partial class ChatsUsuarios
{
    public string IdUsuario { get; set; } = null!;

    public Guid IdChat { get; set; }

    public virtual Chats IdChatNavigation { get; set; } = null!;

    public virtual ICollection<Mensajes> Mensajes { get; set; } = new List<Mensajes>();
}
