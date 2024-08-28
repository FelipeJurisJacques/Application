export class IndexedDb {

    /**
     * @var IDBOpenDBRequest
     */
    #connection

    #dotNetObject

    static open(dotNetObject, name, upgrade = null) {
        console.log(upgrade)
        const instance = new this(dotNetObject)
        return new Promise((resolve, reject) => {
            if (upgrade && upgrade.version) {
                instance.#connection = window.indexedDB.open(name, upgrade.version)
                instance.#connection.onupgradeneeded = event => {
                    const connection = event.target.result
                    for (let store of upgrade.stores) {
                        let storage
                        if (connection.objectStoreNames.contains(store.name)) {
                            console.log(connection)
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
                    }
                }
            } else {
                instance.#connection = window.indexedDB.open(name)
            }
            instance.#connection.onerror = event => {
                console.error(event)
                reject(event.target.error)
            }
            instance.#connection.onsuccess = event => {
                console.log(event)
                resolve(instance)
            }
        })
    }

    constructor(dotNetObject) {
        this.#dotNetObject = dotNetObject
    }
}