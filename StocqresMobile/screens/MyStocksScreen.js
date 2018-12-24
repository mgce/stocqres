import React, { Component } from "react";
import { connect } from "react-redux";
import { Container, Content, Text, Spinner } from "native-base";
import { goToAuth, goToHome } from "./navigation";
import { AsyncStorage } from "react-native";
import constants from "../common/constants";

class MyStocksScreen extends Component{ 
    render(){
        return(
            <Container>
                <Content>
                    <Text>test</Text>
                </Content>
            </Container>
        )
    }
}

export default connect()(MyStocksScreen);