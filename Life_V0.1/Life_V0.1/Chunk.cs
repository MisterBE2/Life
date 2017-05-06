using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Life_V0._1
{
    public partial class Chunk
    {
        public Point index { get; set; }
        public Rectangle chunk { get; set; }
        public bool isLit = false;
        public bool isSearched = false;

        //List<Block> blocks = new List<Block>();
        List<Creature> creatures = new List<Creature>(); // Creatures walking on this chunk

        #region Constructor

        /// <summary>
        /// Creates new instance of chunk
        /// </summary>
        /// <param name="_chunk">Rectangle - chunk</param>
        public Chunk(Rectangle _chunk, Point _index)
        {
            chunk = _chunk;
            index = _index;
        }

        /// <summary>
        /// Creates new instance of chunk
        /// </summary>
        /// <param name="position">Point in which chunk starts</param>
        /// <param name="size">Size of chunk</param>
        public Chunk(Point position, Size size, Point _index)
        {
            chunk = new Rectangle(position, size);
            index = _index;
        }

        #endregion

        #region Chunk

        /// <summary>
        /// Tells program if it needs to lit chunk
        /// </summary>
        /// <param name="state">Lit state</param>
        public void Lit(bool state)
        {
            isLit = state;
        }

        /// <summary>
        /// Updates index
        /// </summary>
        /// <param name="_index">Position in greed</param>
        public void SetIndex(Point _index)
        {
            index = _index;
        }

        /// <summary>
        /// Tels chunk if it is being searched
        /// </summary>
        /// <param name="state">Sets search status</param>
        public void Searched(bool state)
        {
            isSearched = state;
        }

        #endregion

        #region Creatures

        /// <summary>
        /// Checks if given creature is inside chunk
        /// </summary>
        /// <param name="creature">Checked creature</param>
        /// <returns></returns>
        public bool ContainCreature(Creature creature)
        {
            return chunk.Contains((int)(creature.Position.X), (int)(creature.Position.Y));
        }

        /// <summary>
        /// Tells if chunk has any creatures
        /// </summary>
        /// <returns></returns>
        public bool ContainAnyCreature()
        {
            return creatures.Count > 0;
        }

        /// <summary>
        /// Checks if given creature is alredey assigned to this chunk
        /// </summary>
        /// <param name="creature">Checked creature</param>
        /// <returns></returns>
        public bool CreatureInList(Creature creature)
        {
            return creatures.Contains(creature);
        }

        /// <summary>
        /// Adds given creature if it isn't already assigned
        /// </summary>
        /// <param name="creature">Creature to assign</param>
        /// <returns></returns>
        public bool AddCreature(Creature creature)
        {
            if (ContainCreature(creature) && !CreatureInList(creature))
            {
                creatures.Add(creature);
                creature.AttachChunk(this);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Removes given creature
        /// </summary>
        /// <param name="creature">Creature to remove</param>
        /// <returns></returns>
        public void RemoveCreature(Creature creature)
        {
            creatures.Remove(creature);
            creature.RemoveChunk(this);
            creature.FlagChunkLast(this);
            Lit(false);
        }

        /// <summary>
        /// Checks if creature is still in chunk. If not deletes them.
        /// </summary>
        /// <param name="creature">Creature to check</param>
        /// <returns></returns>
        public bool UpdateCreature(Creature creature)
        {
            if (!ContainCreature(creature))
            {
                creatures.Remove(creature);
                creature.FlagChunkLast(this);
                creature.RemoveChunk(this);
                Lit(false);
                return false;
            }
            else
            {
                Lit(creature.litChunkOnEntry);
                return true;
            }
        }

        /// <summary>
        /// Checks if assigned creatures are in chunk. If not deletes them.
        /// </summary>
        public void UpdateAllCreatures()
        {
            for (int i = creatures.Count; i >= 0; i++)
            {
                Creature c = creatures[i];

                if (!ContainCreature(c))
                {
                    creatures.Remove(c);
                    c.RemoveChunk(this);
                    c.FlagChunkLast(this);
                    Lit(false);
                }
                else
                    Lit(c.litChunkOnEntry);
            }
        }
        #endregion
    }
}
