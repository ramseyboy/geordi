package com.ramseyboy.geordi

/**
 * Created by walkerhannan on 4/22/14.
 */
class GeordiApp {

    def gen() {
        def generator = new TestGenerator();
        generator.generateTests("/bdd_parser_test.yaml");
    }

    static void main(String... args)  {
        GeordiApp app = new GeordiApp()
        app.gen();
    }
}
