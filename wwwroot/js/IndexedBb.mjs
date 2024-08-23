export class IndexedBb {
    static #connections = []

    /**
     * @var IDBOpenDBRequest
     */
    #connection

    #dotNetObject

    static open(dotNetObject, name, version) {
        const instance = new IndexedBD(dotNetObject)
        instance.#connection = window.indexedDB.open(name, version)
        return instance
    }

    constructor(dotNetObject) {
        this.#dotNetObject = dotNetObject
        IndexedBD.#connections.push(this)
    }
}