package com.ramseyboy.geordi.model

/**
 * Created by walkerhannan on 4/17/14.
 */
class Specification {
    String Specification
    List<Feature> Features

    def String getSpecName() {
        return Specification
    }

    def List<Feature> getFeatures() {
        return Features
    }
}
