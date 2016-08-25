package me.ramseyboy.geordi

import groovy.transform.CompileStatic

@CompileStatic
class GeordiApp implements Runnable {

    static void main(String... args) {
        new GeordiApp().run()
    }

    @Override
    void run() {
        def generator = new TestGenerator();
        generator.generateTests("/bdd_parser_test.json");
    }
}
