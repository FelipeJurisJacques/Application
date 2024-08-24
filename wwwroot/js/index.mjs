import { IndexedDb } from "./IndexedDb.mjs"

class Main {
    #indexedDb
    #dotNetObject

    constructor() {
        this.#indexedDb = IndexedDb
        this.#dotNetObject = null
    }

    get indexedDb() {
        return this.#indexedDb
    }

    initialize(dotNetObject) {
        if (!this.#dotNetObject) {
            this.#dotNetObject = dotNetObject
            window.addEventListener('resize', event => {
                dotNetObject.invokeMethodAsync('OnEvent', event.type)
            })
        }
    }
}

window.interop = new Main()