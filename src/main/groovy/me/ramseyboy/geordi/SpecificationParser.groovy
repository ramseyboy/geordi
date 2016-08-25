package me.ramseyboy.geordi

import groovy.json.JsonSlurper
import groovy.transform.CompileStatic
import me.ramseyboy.geordi.model.Block
import me.ramseyboy.geordi.model.Feature
import me.ramseyboy.geordi.model.Specification

class SpecificationParser {

    private final JsonSlurper jsonSlurper;

    SpecificationParser() {
        jsonSlurper = new JsonSlurper()
    }

    Specification parse(String fileName) {

        def obj = jsonSlurper.parse(this.getClass().getResource(fileName))
        assert obj instanceof Map

        def json = (Map) obj

        def specification = (String) json.get("specification")
        def featureObjs = (Object[]) json.get("features")

        List<Feature> features = featureObjs.collect() {
            def featureJson = (Map) it
            def featureName = (String) featureJson.get("feature")
            def blockJson = (Map) featureJson.get("block")

            def block = new Block(
                    given: blockJson.get("given"),
                    when: blockJson.get("when"),
                    then: blockJson.get("then")
            )
            return new Feature(feature: featureName, block: block)
        }

        return new Specification(specification: specification, features: features)
    }
}
