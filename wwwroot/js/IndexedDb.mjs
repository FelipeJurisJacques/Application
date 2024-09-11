export class IDBFactory {

    /**
     * @var IDBDatabase
     */
    #connection

    #dotNetObject

    static open(dotNetObject, name, version = null) {
        if (version) {
            return new IDBOpenDBRequest(dotNetObject, window.indexedDB.open(name, version))
        } else {
            return new IDBOpenDBRequest(dotNetObject, window.indexedDB.open(name))
        }
    }

    constructor(dotNetObject, connection) {
        this.#connection = connection
        this.#dotNetObject = dotNetObject
        this.#connection.onclose = event => {
            this.#dotNetObject.invokeMethodAsync('OnEvent', event.type)
        }
    }
}

class IDBOpenDBRequest {

    /**
     * @var Promise
     */
    #upgrade

    /**
     * @var IDBOpenDBRequest
     */
    #resource

    /**
     * @var IDBDatabase
     */
    #connection

    /**
     * @var IDBTransaction
     */
    #transaction

    #dotNetObject

    constructor(dotNetObject, resource) {
        this.#upgrade = new Promise()
        this.#resource = resource
        this.#dotNetObject = dotNetObject
        this.#resource.onupgradeneeded = async event => {
            this.#connection = event.target.result
            this.#transaction = event.target.transaction
            this.#dotNetObject.invokeMethodAsync('OnEvent', event.type)
            await this.#upgrade
        }
        this.#resource.onerror = event => {
            this.#dotNetObject.invokeMethodAsync('OnEvent', event.type)
        }
        this.#resource.onsuccess = event => {
            this.#dotNetObject.invokeMethodAsync('OnEvent', event.type)
        }
    }

    transaction(dotNetObject, names, mode, options = null) {
        return new IndexedDbTransaction(dotNetObject, options ? this.#connection.transaction(
            names,
            mode,
            options
        ) : this.#connection.transaction(
            names,
            mode
        ))
    }
}

class IDBUpgrade extends Promise {
    #reject
    #resolve

    constructor() {
        super((resolve, reject) => {
            this.#reject = reject
            this.#resolve = resolve
        })
    }
}

class IndexedDbTransaction {

    /**
     * @var IDBTransaction
     */
    #transaction

    #dotNetObject

    constructor(dotNetObject, transaction) {
        this.#transaction = transaction
        this.#dotNetObject = dotNetObject
        this.#transaction.onerror = event => {
            this.#dotNetObject.invokeMethodAsync('OnEvent', event.type)
        }
        this.#transaction.onabort = event => {
            this.#dotNetObject.invokeMethodAsync('OnEvent', event.type)
        }
        this.#transaction.oncomplete = event => {
            this.#dotNetObject.invokeMethodAsync('OnEvent', event.type)
        }
    }
}

/*
for (let store of upgrade.stores) {
    let storage
    if (connection.objectStoreNames.contains(store.name)) {
        storage = transaction.objectStore(store.name)
    } else {
        let options = {}
        if (store.keyPath) {
            options.keyPath = store.keyPath
            if (store.autoIncrement) {
                options.autoIncrement = store.autoIncrement
            }
        }
        storage = connection.createObjectStore(store.name, options)
    }
    for (let index of store.indexes) {
        if (!storage.indexNames.contains(index.name)) {
            storage.createIndex(index.name, index.name, {
                unique: index.unique ? true : false,
                multiEntry: index.multiEntry ? true : false,
            })
        }
    }
}*/