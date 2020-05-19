using System.Collections.Generic;

namespace PandaMonogame
{
    public class Entity2DList
    {
        protected List<Entity2D> _entities = new List<Entity2D>();

        public Entity2DList()
        {
            
        }

        public Entity2D this[int index]
        {
            get
            {
                return _entities[index];
            }

            set
            {
                _entities[index] = value;
            }
        }

        public void Add(Entity2D entity)
        {
            _entities.Add(entity);
        }

        public void Remove(int index)
        {
            _entities.Remove(_entities[index]);
        }

        public void Remove(Entity2D entity)
        {
            _entities.Remove(entity);
        }

        public void Clear()
        {
            _entities.Clear();
        }
    }
}
