using System;
using System.Collections.Generic;
using System.Text;

namespace truYumRestaurant
{
    class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
    }
}
