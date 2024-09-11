// import { IDBFactory } from "./IndexedDb.mjs"

IDBDatabase.prototype.callGetterAttribute = function (key) {
    return this[key]
}

IDBDatabase.prototype.callSetterAttribute = function (key, value) {
    this[key] = value
}

IDBOpenDBRequest.prototype.callGetterAttribute = function (key) {
    return this[key]
}

IDBOpenDBRequest.prototype.callSetterAttribute = function (key, value) {
    this[key] = value
}

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