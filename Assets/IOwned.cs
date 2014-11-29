using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Players
{
    public abstract class IOwned
    {
        private Player owner;
        public Player Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                this.owner.Owned.Remove(this);
                value.Owned.Add(this);
                this.owner = value;
            }
        }
    }
}