import { IndexedBb } from "./IndexedBb.mjs"

class Main {
    #indexedDb
    #dotNetObject

    constructor() {
        this.#indexedDb = IndexedBb
        this.#dotNetObject = null
    }

    get indexedBD() {
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