import { IndexedDb } from "./IndexedDb.mjs"

window.interop = class Main {
    static #dotNetObject = null

    static get indexedDb() {
        return IndexedDb
    }

    static initialize(dotNetObject) {
        if (!this.#dotNetObject) {
            this.#dotNetObject = dotNetObject
            window.addEventListener('resize', event => {
                dotNetObject.invokeMethodAsync('OnEvent', event.type)
            })
        }
    }
}

console.log('initializing...')