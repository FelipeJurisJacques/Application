export class IndexedBb {
    static #connections = []

    /**
     * @var IDBOpenDBRequest
     */
    #connection

    #dotNetObject

    static open(dotNetObject, tcs, name, version = null) {
        const instance = new IndexedBb(dotNetObject)
        if (version) {
            instance.#connection = window.indexedDB.open(name, version)
        } else {
            instance.#connection = window.indexedDB.open(name, version)
        }
        instance.#connection.onerror = event => {
            tcs.error(event.target.error)
        }
        instance.#connection.onsuccess = event => {
            tcs.complete(event)
        }
        return instance
    }

    constructor(dotNetObject) {
        this.#dotNetObject = dotNetObject
        IndexedBb.#connections.push(this)
    }
}