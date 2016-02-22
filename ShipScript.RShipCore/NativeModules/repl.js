'use strict';
// ReSharper disable UndeclaredGlobalVariableUsing
const core = require('core');
const console = require('console');
const explore = require('explore');

function evaluate(command) {
    if (typeof command !== 'string') {
        return;
    }

    EngineInternal.evalCode = command;
    const success = core.eval();
    EngineInternal.evalCode = undefined;
    if (success) {
        explore(EngineInternal.evalResult);
        EngineInternal.evalResult = undefined;
    } else {
        console.err(EngineInternal.evalError);
        EngineInternal.evalError = undefined;
    }
}

function hook(stream) {
    return stream.pipe(evaluate);
}

exports.hook = hook;
