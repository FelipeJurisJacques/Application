export class IndexedDb {

    /**
     * @var IDBOpenDBRequest
     */
    #connection

    #dotNetObject

    static open(dotNetObject, name, upgrade = null) {
        const instance = new this(dotNetObject)
        return new Promise((resolve, reject) => {
            if (upgrade && upgrade.version) {
                instance.#connection = window.indexedDB.open(name, upgrade.version)
                instance.#connection.onupgradeneeded = event => {
                    const connection = event.target.result
                    const transaction = event.target.transaction
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
                    }
                }
            } else {
                instance.#connection = window.indexedDB.open(name)
            }
            instance.#connection.onerror = event => {
                reject(event.target.error)
            }
            instance.#connection.onsuccess = event => {
                resolve(instance)
            }
        })
    }

    constructor(dotNetObject) {
        this.#dotNetObject = dotNetObject
    }

    transaction(dotNetObject, names, mode, options = null) {
        return new IndexedDbTransaction(
            dotNetObject,
            options ? this.#connection.transaction(
                names,
                mode,
                options
            ) : this.#connection.transaction(
                names,
                mode
            )
        )
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
    }
}