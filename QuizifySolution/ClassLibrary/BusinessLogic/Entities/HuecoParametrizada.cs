﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizify.Entities
{
    public partial class HuecoParametrizada
    {
        public HuecoParametrizada()
        {

        }

        public HuecoParametrizada(PreguntaParametrizada preguntaCorrespondiente)
        {
            PreguntaCorrespondiente = preguntaCorrespondiente;
        }

    }
}