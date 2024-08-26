export class IndexedDb {

    /**
     * @var IDBOpenDBRequest
     */
    #connection

    #dotNetObject

    static open(dotNetObject, name, version = null, upgrade = null) {
        const instance = new this(dotNetObject)
        return new Promise((resolve, reject) => {
            if (version) {
                instance.#connection = window.indexedDB.open(name, version)
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