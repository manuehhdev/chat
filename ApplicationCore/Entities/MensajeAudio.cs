using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities;

public partial class MensajeAudio
{
    public Guid Id { get; set; }

    public string UrlAudio { get; set; } = null!;

    public virtual Mensajes IdNavigation { get; set; } = null!;
}
