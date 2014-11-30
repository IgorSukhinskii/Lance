using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameWorld.Players
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
                if (this.owner != null)
                {
                    this.owner.Owned.Remove(this);
                    value.Owned.Add(this);
                }
                this.owner = value;
            }
        }
    }
}