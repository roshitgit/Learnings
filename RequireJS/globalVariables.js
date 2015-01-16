You would make that a module-level variable. For example,

// In foo.js
define(function () {
    var theFoo = {};

    return {
        getTheFoo: function () { return theFoo; }
    };
});

// In bar.js
define(["./foo"], function (foo) {
    var theFoo = foo.getTheFoo(); // save in convenience variable

    return {
        setBarOnFoo: function () { theFoo.bar = "hello"; }
    };
}

// In baz.js
define(["./foo"], function (foo) {
    // Or use directly.
    return {
        setBazOnFoo: function () { foo.getTheFoo().baz = "goodbye"; }
    };
}

// In any other file
define(["./foo", "./bar", "./baz"], function (foo, bar, baz) {
    bar.setBarOnFoo();
    baz.setBazOnFoo();

    assert(foo.getTheFoo().bar === "hello");
    assert(foo.getTheFoo().baz === "goodbye");
};
