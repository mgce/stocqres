import React, {Component} from 'react';
import { StyleSheet, FlatList, View, TouchableOpacity } from 'react-native';
import { Container, Header, Content, Left, Body, Title, Form, Item, Input, Label, Button, Text } from 'native-base';
import {connect} from 'react-redux';

class MyListItem extends React.PureComponent{
    // _onPress = () => {
    //     this.props.onPressItem(this.props.id);
    // }
    render(){
        return(
            <TouchableOpacity>
                <View style={listItemStyles.container}>
                    <View style={listItemStyles.company}>
                        <Text>{this.props.code}</Text>
                        <Text style={listItemStyles.companyName}>
                            {this.props.name}
                        </Text>
                    </View>
                    <View style={listItemStyles.row}> 
                        <View style={listItemStyles.price}>
                            <Text>102.080</Text>
                        </View>
                        <Button primary>
                            <Text>Buy</Text>
                        </Button>
                    </View>
                </View>
            </TouchableOpacity>
        )
    }
}

const listItemStyles = StyleSheet.create({
    container:{
        alignItems: 'center',
        flex: 1,
        flexDirection: 'row',
        justifyContent: 'space-between',
        // marginTop: 10,
        // marginBottom: 10
        padding: 10
    },
    company:{
        flexDirection: 'column'
    },
    companyName:{
        fontSize: 10
    },
    row:{
        flexDirection: 'row',
        alignItems: 'center'
    },
    price:{
        fontSize: 16,
        fontWeight: 'bold',
        paddingLeft: 10
    }
})

export default class MainScreen extends Component{
    constructor(props){
        super(props);
    }
    _renderItem = ({item}) =>(
        <MyListItem
        id={item.id}
        code={item.code}
        name={item.name}/>
    )
    render(){
        return(
            <Container>
                <Header>
                <Left/>
                    <Body>
                        <Title>Stocks</Title>
                    </Body>
                </Header>
                <Content>
                    <FlatList 
                        data={items}
                        renderItem={this._renderItem}
                        />
                </Content>
            </Container>
        )
    }
}

const items = [{id:1, code: 'ABC', name:'Abece'}, {id:2, code: 'DEF', name:'Deeef'}];