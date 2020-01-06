﻿using Phanerozoic.Core.Entities;
using System.Collections.Generic;

namespace Phanerozoic.Core.Services
{
    public interface INotifyer
    {
        void Notify(IList<MethodEntity> methodList);
    }
}