﻿using System;

namespace Glimpse
{
    public interface IMessageEnvelope
    {
        string Type { get; }

        string Message { get; }
    }
}