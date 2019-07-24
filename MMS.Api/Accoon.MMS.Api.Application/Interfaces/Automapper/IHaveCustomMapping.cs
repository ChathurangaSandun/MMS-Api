using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accoon.MMS.Api.Application.Interfaces.Automapper
{
    public interface IHaveCustomMapping
    {
        void CreateMappings(Profile configuration);
    }
}
