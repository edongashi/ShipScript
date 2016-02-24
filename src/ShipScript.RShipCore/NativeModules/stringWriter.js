'use strict';
// ReSharper disable UndeclaredGlobalVariableUsing
const string = Symbol();

class StringWriter {
    constructor() {
        this[string] = '';
    }

    write(text) {
        if (text) {
            this[string] += text;
        }
    }

    writeln(text) {
        if (text) {
            this[string] += text + '\n';
        } else {
            this[string] += '\n';
        }
    }

    getText() {
        return this[string];
    }

    clear() {
        this[string] = '';
    }
}

module.exports = StringWriter;
