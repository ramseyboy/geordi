package com.ramseyboy.geordi

/**
 * Created by walkerhannan on 4/16/14.
 */
class Main {

    static main(args)  {
        def gen = new Generator()
        gen.generateTests('/bdd_parser_test.yaml')
    }
}
