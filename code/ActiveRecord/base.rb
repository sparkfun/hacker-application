module ActiveRecord
  class Base < NSManagedObject
    class << self
      def all
        find
      end
      
      def load_with_uri(uri)
        store = ActiveRecord::Store.context.persistentStoreCoordinator
        object_id = store.managedObjectIDForURIRepresentation(NSURL.URLWithString(uri))
        ActiveRecord::Store.context.existingObjectWithID(object_id, error:nil)
      end

      def where(criteria)
        find(criteria, :predicate)
      end
      
      def order(criteria)
        find(criteria, :sort)
      end
      
      def sql(criteria)
        find(criteria, :sql)
      end

      def delete_all
        self.all.each do |record|
          ActiveRecord::Store.context.deleteObject(record)
        end
        ActiveRecord::Store.context.save(nil)
      end

      def new_record(defaults={})
        object = NSManagedObject.alloc.initWithEntity(entity, insertIntoManagedObjectContext:ActiveRecord::Store.context)
        defaults.each { |k, v| object.setValue(v, forKey:k) }
        object
      end

      private

      def build_predicate_query(criteria)
        "".tap do |query|
          criteria.each_with_index do |pair, index|
            query << "#{pair.first.to_s}='#{pair.last.to_s}'"
            query << ' and ' unless criteria.size == index+1
          end
        end
      end

      def find(criteria={}, type=nil)
        request = NSFetchRequest.alloc.init
        request.entity = entity

        unless criteria.empty?
          if type == :predicate
            request.predicate = NSPredicate.predicateWithFormat(build_predicate_query(criteria))
          elsif type == :sql
            request.predicate = NSPredicate.predicateWithFormat(criteria)
          else
            ascending = criteria.values.first == :ascending ? true : false
            request.sortDescriptors = [NSSortDescriptor.sortDescriptorWithKey(criteria.keys.first, ascending:ascending)]
          end
        end

        error_ptr = Pointer.new(:object)
        data = ActiveRecord::Store.context.executeFetchRequest(request, error:error_ptr)
        raise "Error fetching objects #{error_ptr[0].description}" if data.nil?
        data
      end

      def entity
        NSEntityDescription.entityForName(self.to_s, inManagedObjectContext:ActiveRecord::Store.context)
      end
    end

    def new_record?
      self.objectID.isTemporaryID
    end

    def destroy
      before_destroy if respond_to?(:before_destroy)
      ActiveRecord::Store.context.deleteObject(self)
      save
    end

    def save
      record_deleted = self.isDeleted
      ActiveRecord::Store.context.insertObject(self) if self.new_record? && !record_deleted
      error_ptr = Pointer.new(:object)
      if !ActiveRecord::Store.context.save(error_ptr)
        raise "Error saving model: #{error_ptr[0].description}"
      else
        after_save if respond_to?(:after_save) && !record_deleted
      end
    end
  end
end