// import { IDBFactory } from "./IndexedDb.mjs"

import { } from "./interoperability.mjs"

window.interop = class Main {
    static #dotNetObject = null

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