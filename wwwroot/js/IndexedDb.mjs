export class IndexedDb {
    static #connections = []

    /**
     * @var IDBOpenDBRequest
     */
    #connection

    #tcs
    #dotNetObject

    static open(dotNetObject, tcs, name) {
        const instance = new this(dotNetObject, tcs)
        instance.#connection = window.indexedDB.open(name)
    }

    static upgrade(dotNetObject, tcs, name, version) {
        const instance = new this(dotNetObject, tcs)
        instance.#connection = window.indexedDB.open(name, version)
    }

    constructor(dotNetObject, tcs) {
        this.#tcs = tcs
        this.#dotNetObject = dotNetObject
        IndexedDb.#connections.push(this)
        this.#connection.onerror = event => {
            tcs.error(event.target.error)
        }
        this.#connection.onsuccess = event => {
            tcs.complete(event)
        }
    }
}