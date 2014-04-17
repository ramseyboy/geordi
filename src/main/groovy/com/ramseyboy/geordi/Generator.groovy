package com.ramseyboy.geordi

import groovy.text.GStringTemplateEngine
import org.yaml.snakeyaml.Yaml

/**
 * Created by walkerhannan on 4/16/14.
 */
public class Generator {

    def generateTests(String reqFileName) {
        String specs = supplySpecs(reqFileName)
        new File("src/main/resources/results.groovy").withWriter { out ->
            out.writeLine(supplyClass(specs).toString())
        }
    }

    def supplyClass(String specs) {
        def classTemplate = this.getClass().getResource('/spec.template')

        def binding = [methods:specs]
        def engine = new GStringTemplateEngine()
        return engine.createTemplate(classTemplate).make(binding)
    }

    def supplySpecs(String reqFileName) {

        Yaml yaml = new Yaml();
        List<String> reqList = yaml.load(this.getClass().getResource('/bdd_parser_test.yaml').text)

        def methodTemplate = this.getClass().getResource('/method.template')

        def spockMethods = new ArrayList<String>()

        reqList.eachWithIndex { obj, i ->
            def binding = [requirement:obj]
            def engine = new GStringTemplateEngine()
            def template = engine.createTemplate(methodTemplate).make(binding)
            spockMethods.add(template.toString())
        }

        def methodBody = ""
        spockMethods.each { str ->
            methodBody += str
        }

        return methodBody
    }
}