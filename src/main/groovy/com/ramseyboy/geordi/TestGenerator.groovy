package com.ramseyboy.geordi

import com.ramseyboy.geordi.model.Feature
import com.ramseyboy.geordi.model.Specification
import groovy.text.GStringTemplateEngine

/**
 * Created by walkerhannan on 4/16/14.
 */
class TestGenerator {

    def generateTests(String reqFileName) {
        Specification spec = new YamlParser(fileName:reqFileName).parse()

        List<Feature> reqList = spec.getFeatures()
        String feature = "${spec.getSpecName()}${if (!spec.getSpecName().contains("Spec")) "Spec"}"

        String specs = supplySpecs(reqList)

        def specification = supplyClass(feature, specs).toString()

        new File("src/test/groovy/${feature}.groovy")
                .withWriter { out ->
            out.writeLine(specification)
        }
    }

    def supplyClass(String specName, String specs) {
        def classTemplate = this.getClass().getResource('/templates/spec.template')

        def binding = [className:specName, methods:specs]
        def engine = new GStringTemplateEngine()
        return engine.createTemplate(classTemplate).make(binding)
    }

    def supplySpecs(List<Feature> reqList) {
        def methodTemplate = this.getClass().getResource('/templates/method.template')

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