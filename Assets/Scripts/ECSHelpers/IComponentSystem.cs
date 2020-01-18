using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.ECSHelpers
{
    public interface IComponentSystem
    {
        EntityManager CurrentEntityManager { get; set; }

        void OnUpdate();
        void OnPostUpdate();
    }
}
