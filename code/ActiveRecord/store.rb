module ActiveRecord
  class Store
    DB_NAME = 'test_db'
    MODEL_RESOURCE_NAME = 'data_model'
    
    def self.context
      @context ||= begin
        store_url = NSURL.fileURLWithPath(File.join(NSHomeDirectory(), 'Documents', "#{DB_NAME}.sqlite"))
        model_url = NSBundle.mainBundle.URLForResource(MODEL_RESOURCE_NAME, withExtension:"momd")
        managedObjectModel = NSManagedObjectModel.alloc.initWithContentsOfURL(model_url)
        
        options = {
          NSMigratePersistentStoresAutomaticallyOption => true,
          NSInferMappingModelAutomaticallyOption => true
        }

        error_ptr = Pointer.new(:object)
        store = NSPersistentStoreCoordinator.alloc.initWithManagedObjectModel(managedObjectModel)
        if !store.addPersistentStoreWithType(NSSQLiteStoreType, configuration:nil, URL:store_url, options:options, error:error_ptr)
          raise "Can't add persistent SQLite store: #{error_ptr[0].description}"
        end

        context = NSManagedObjectContext.alloc.init
        context.persistentStoreCoordinator = store
        context
      end
    end
  end
end