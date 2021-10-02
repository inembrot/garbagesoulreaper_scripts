using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MaPremiereApplication.Sources
{
    class Dechet : Bloc
    {
        //Initialisation des variables
        public Dechet(Bloc bloc)
        {
            m_bloc = bloc.m_bloc;
            m_position = bloc.m_position;
            m_code = bloc.m_code;
            m_visible = bloc.m_visible;
            m_traversable = bloc.m_traversable;
        }

        public override String ToString()
        {
            switch (m_code)
            {
                case 21:
                    return ("Glass");
                case 22:
                    return ("Recyclable except glass");
                case 23:
                    return ("Non recyclable");
                default:
                    return ("None");
            }
        }
    }
}
