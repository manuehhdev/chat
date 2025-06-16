using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class Mensajes
{
    public Guid Id { get; set; }

    public string IdUsuario { get; set; } = null!;

    public Guid IdChat { get; set; }

    public DateTime FechaEnvio { get; set; }

    public bool Leido { get; set; }

    public virtual ChatsUsuarios ChatsUsuarios { get; set; } = null!;

    public virtual MensajeAudio? MensajeAudio { get; set; }

    public virtual MensajeImagen? MensajeImagen { get; set; }

    public virtual MensajeTexto? MensajeTexto { get; set; }

    public virtual MensajeUbicacion? MensajeUbicacion { get; set; }
}
