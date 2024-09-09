export class IndexedDb {

    /**
     * @var IDBDatabase
     */
    #connection

    #dotNetObject

    static open(dotNetObject, name, upgrade = null) {
        return new Promise((resolve, reject) => {
            let open // IDBOpenDBRequest
            if (upgrade && upgrade.version) {
                open = window.indexedDB.open(name, upgrade.version)
                open.onupgradeneeded = event => {
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
                open = window.indexedDB.open(name)
            }
            open.onerror = event => {
                reject(event.target.error)
            }
            open.onsuccess = event => {
                resolve(new this(dotNetObject, event.target.result))
            }
        })
    }

    constructor(dotNetObject, connection) {
        this.#connection = connection
        this.#dotNetObject = dotNetObject
        this.#connection.onclose = event => {
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