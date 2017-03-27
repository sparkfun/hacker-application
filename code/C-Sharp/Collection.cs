using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;

namespace Utils
{
    public class _Collection
    {
        // To use - your class must implement this interface
        public interface IOrderable<T>
        {
            bool DeleteFromCollection(int idx);
            T ReorderItem(int idx, string direction);
            T AddToCollection(List<Pair> argList);
        }

        public static T AddItemToOrderedCollection<T>(List<T> list, List<Pair> args) where T : new()
        {
            return AddItemToOrderedCollection<T>(list, args, "IDisplayOrder", false);
        }
        public static T AddItemToOrderedCollection<T>(List<T> list, List<Pair> args, bool addAtStart) where T : new()
        {
            return AddItemToOrderedCollection<T>(list, args, "IDisplayOrder", addAtStart);
        }
        public static T AddItemToOrderedCollection<T>(List<T> list, List<Pair> args, string ordinalColumnName, bool addAtStart) where T : new()
        {

            T newItem = new T();

            if(list.Count > 1)
                list.Sort(new Utils.Reflector.CompareEntities<T>(Utils.Reflector.Direction.Ascending, ordinalColumnName));

            if (addAtStart)
            {
                //move all items up one and then stick at start
                IncrementHigherOrderItems(list, -1, "IDisplayOrder");
                Utils.Reflector.AssignToExpression(newItem, ordinalColumnName, 0);
            }
            else
            {
                //get max value of idisplayorder and add one
                int listCount = list.Count;
                int highestOrdinal = 0;
                if (listCount > 0)
                {
                    T lastItem = list[listCount - 1];
                    highestOrdinal = (int)Utils.Reflector.EvaluateExpression(lastItem, ordinalColumnName);
                    highestOrdinal++;
                }

                Utils.Reflector.AssignToExpression(newItem, ordinalColumnName, highestOrdinal);
            }

            foreach (Pair p in args)
                Utils.Reflector.AssignToExpression(newItem, p.First.ToString(), p.Second);

            SaveObject(newItem);
            list.Add(newItem);

            return newItem;

        }

        /// <summary>
        /// A return value of true indicates that the collection has been correctly updated
        /// </summary>
        public static bool DeleteFromOrderedCollection<T>(List<T> list, int idx)
        {
            return DeleteFromOrderedCollection<T>(list, idx, "IDisplayOrder");
        }
        public static bool DeleteFromOrderedCollection<T>(List<T> list, int idx, string ordinalColumnName)
        {
            //ensure the object is in the collection
            T existingObject = list.Find(delegate(T match) { return ((int)Utils.Reflector.EvaluateExpression(match, "Id") == idx); } );

            if (existingObject != null)
            {
                //get the ordinal of the current element
                int currentOrdinal = (int)Utils.Reflector.EvaluateExpression(existingObject, ordinalColumnName);

                //the order of deletion is important here
                //we must first delete the object before reassigning display order values to the remaining elements
                Type b = existingObject.GetType().BaseType;
                b.InvokeMember("Delete", System.Reflection.BindingFlags.InvokeMethod,
                   null, null, new object[] { idx });

                //remove the existing object from the collection //this.Remove(entity);
                list.Remove(existingObject);

                //decrement higher order elements only if the element is not the last element
                if(currentOrdinal < list.Count)
                    DecrementHigherOrderItems(list, currentOrdinal, ordinalColumnName);

                return true;
            }

            return false;
        }

        private static void DecrementHigherOrderItems<T>(List<T> list, int ordinal, string ordinalColumnName)
        {
            //get a collection of higher order items
            List<T> higherOrdered = list.FindAll(delegate(T match) { return ((int)Utils.Reflector.EvaluateExpression(match, ordinalColumnName) > ordinal); } );

            //make sure we save "up the ladder" so that unique displayorder values are valid
            //sort by display order
            higherOrdered.Sort(new Utils.Reflector.CompareEntities<T>(Utils.Reflector.Direction.Ascending, ordinalColumnName));

            //decrement the higher order items
            foreach (T listItem in higherOrdered)
            {
                int newOrdinal = (int)Utils.Reflector.EvaluateExpression(listItem, ordinalColumnName) - 1;
                SaveValue(listItem, ordinalColumnName, newOrdinal);
            }
        }

        private static void IncrementHigherOrderItems<T>(List<T> list, int ordinal, string ordinalColumnName)
        {
            //get a collection of higher order items
            List<T> higherOrdered = list.FindAll(delegate(T match) { return ((int)Utils.Reflector.EvaluateExpression(match, ordinalColumnName) > ordinal); });

            //make sure we save "up the ladder" so that unique displayorder values are valid
            //sort by display order
            higherOrdered.Sort(new Utils.Reflector.CompareEntities<T>(Utils.Reflector.Direction.Descending, ordinalColumnName));

            //decrement the higher order items
            foreach (T listItem in higherOrdered)
            {
                int newOrdinal = (int)Utils.Reflector.EvaluateExpression(listItem, ordinalColumnName) + 1;
                SaveValue(listItem, ordinalColumnName, newOrdinal);
            }
        }

        public static T ReorderOrderedCollection<T>(List<T> list, int idx, string direction)
        {
            return ReorderOrderedCollection<T>(list, idx, direction, "IDisplayOrder");
        }
        public static T ReorderOrderedCollection<T>(List<T> list, int idx, string direction, string ordinalColumnName)
        {
            //ensure the object is in the collection
            T existingObject = list.Find(delegate(T match) { return ((int)Utils.Reflector.EvaluateExpression(match, "Id") == idx); });

            if (existingObject != null)
            {
                //get the ordinal of the current element
                int currentOrdinal = (int)Utils.Reflector.EvaluateExpression(existingObject, ordinalColumnName);

                if (direction.ToLower().Equals("up") && currentOrdinal > 0)
                {
                    //find the next one up
                    T previousObject = list.Find(delegate(T match) { return ((int)Utils.Reflector.EvaluateExpression(match, ordinalColumnName) == currentOrdinal - 1); });

                    if (previousObject != null)
                    {
                        SaveValue(existingObject, ordinalColumnName, -1);
                        SaveValue(previousObject, ordinalColumnName, currentOrdinal);
                        SaveValue(existingObject, ordinalColumnName, currentOrdinal - 1);
                    }
                }
                else if (direction.ToLower().Equals("down") && currentOrdinal != (list.Count - 1))//count is 1 based
                {
                    //find the next one down
                    T nextObject = list.Find(delegate(T match) { return ((int)Utils.Reflector.EvaluateExpression(match, ordinalColumnName) == currentOrdinal + 1); });

                    if (nextObject != null)
                    {
                        SaveValue(existingObject, ordinalColumnName, -1);
                        SaveValue(nextObject, ordinalColumnName, currentOrdinal);
                        SaveValue(existingObject, ordinalColumnName, currentOrdinal + 1);
                    }
                }
            }

            return existingObject;
        }

        private static void SaveValue(object existing, string columnName, int value)
        {
            Utils.Reflector.AssignToExpression(existing, columnName, value);
            SaveObject(existing);
        }
        private static void SaveObject(object item)
        {
            item.GetType().InvokeMember("Save", System.Reflection.BindingFlags.InvokeMethod, null, item, null);
        }
    }
}